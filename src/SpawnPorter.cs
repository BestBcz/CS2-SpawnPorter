using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace SpawnPorter;

public class SpawnPorter : BasePlugin
{
    public override string ModuleName => "SpawnPorter";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "SpawnPorter Team";
    public override string ModuleDescription => "极简出生点传送插件";

    private List<SpawnPoint> _spawnPoints = new();
    private Dictionary<ulong, DateTime> _playerCooldowns = new();
    private const float CooldownSeconds = 2.0f;
    private const float MaxDistance = 100.0f;
    private const float TraceDistance = 200.0f;

    public class SpawnPoint
    {
        public Vector Position { get; set; } = new Vector();
        public QAngle Angles { get; set; } = new QAngle();
    }

    public override void Load(bool hotReload)
    {
        // 延迟2秒后在主线程执行加载出生点
        AddTimer(2.0f, () => LoadSpawnPoints());
        // 启动定时器检测玩家按键
        AddTimer(0.1f, CheckPlayerInput);
        // 注册回合开始事件监听
        RegisterEventHandler<EventRoundStart>(OnRoundStart);
        Console.WriteLine("[SpawnPorter] 插件已加载，定时器已启动");
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        LoadSpawnPoints();
        AddTimer(0.1f, CheckPlayerInput);
        Console.WriteLine("[SpawnPorter] round_start 事件触发，已重新加载出生点并启动定时器");
        return HookResult.Continue;
    }

    private void LoadSpawnPoints()
    {
        _spawnPoints.Clear();
        
        // 读取CT出生点
        var ctSpawns = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("info_player_counterterrorist");
        foreach (var entity in ctSpawns)
        {
            if (entity == null || !entity.IsValid) continue;
            _spawnPoints.Add(new SpawnPoint { Position = entity.AbsOrigin, Angles = entity.AbsRotation });
        }
        
        // 读取T出生点
        var tSpawns = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("info_player_terrorist");
        foreach (var entity in tSpawns)
        {
            if (entity == null || !entity.IsValid) continue;
            _spawnPoints.Add(new SpawnPoint { Position = entity.AbsOrigin, Angles = entity.AbsRotation });
        }
        
        // 如果没有找到特定出生点，尝试通用出生点
        if (_spawnPoints.Count == 0)
        {
            var genericSpawns = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("info_player_start");
            foreach (var entity in genericSpawns)
            {
                if (entity == null || !entity.IsValid) continue;
                _spawnPoints.Add(new SpawnPoint { Position = entity.AbsOrigin, Angles = entity.AbsRotation });
            }
        }
        
        Console.WriteLine($"[SpawnPorter] 已加载 {_spawnPoints.Count} 个出生点");
    }

    private void CheckPlayerInput()
    {
        try
        {
            var players = Utilities.GetPlayers();
            if (players == null || players.Count() == 0) return;
            
            foreach (var player in players)
            {
                if (player == null || !player.IsValid || !player.PawnIsAlive) continue;
                
                // 检查玩家是否按下使用键
                if (player.Buttons.HasFlag(PlayerButtons.Use))
                {
                    Console.WriteLine($"[SpawnPorter] 玩家 {player.PlayerName} 按下了使用键");
                    TryTeleport(player);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SpawnPorter] CheckPlayerInput 错误: {ex.Message}");
        }
        
        // 递归调用实现重复定时器
        AddTimer(0.1f, CheckPlayerInput);
    }

    private void TryTeleport(CCSPlayerController player)
    {
        try
        {
            if (DateTime.Now < _playerCooldowns.GetValueOrDefault(player.SteamID, DateTime.MinValue))
            {
                Console.WriteLine($"[SpawnPorter] 玩家 {player.PlayerName} 在冷却中");
                return;
            }
            
            var nearest = FindNearestSpawn(player);
            if (nearest == null)
            {
                Console.WriteLine($"[SpawnPorter] 未找到最近的出生点");
                return;
            }
            
            if (player.PlayerPawn.Value == null)
            {
                Console.WriteLine($"[SpawnPorter] 玩家Pawn为空");
                return;
            }
            
            float distance = (player.PlayerPawn.Value.AbsOrigin - nearest.Position).Length();
            Console.WriteLine($"[SpawnPorter] 距离最近出生点: {distance}");
            
            if (distance > MaxDistance)
            {
                Console.WriteLine($"[SpawnPorter] 距离太远，无法传送");
                return;
            }
            
            player.PlayerPawn.Value.Teleport(nearest.Position, nearest.Angles, new Vector(0, 0, 0));
            _playerCooldowns[player.SteamID] = DateTime.Now.AddSeconds(CooldownSeconds);
            player.PrintToChat("[SpawnPorter] 已传送到出生点");
            Console.WriteLine($"[SpawnPorter] 玩家 {player.PlayerName} 已传送到出生点");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SpawnPorter] TryTeleport 错误: {ex.Message}");
        }
    }

    private SpawnPoint? FindNearestSpawn(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value == null) return null;
        var pos = player.PlayerPawn.Value.AbsOrigin;
        SpawnPoint? nearest = null;
        float minDist = float.MaxValue;
        foreach (var spawn in _spawnPoints)
        {
            float dist = (pos - spawn.Position).Length();
            if (dist < minDist)
            {
                minDist = dist;
                nearest = spawn;
            }
        }
        return nearest;
    }
} 