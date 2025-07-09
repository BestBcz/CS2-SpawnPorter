@echo off
echo 正在编译 SpawnPorter CS Sharp 插件...

REM 检查是否存在 .NET SDK
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo 错误: 未找到 .NET SDK
    echo 请确保已安装 .NET 8.0 或更高版本
    pause
    exit /b 1
)

REM 创建输出目录
if not exist "bin" mkdir bin

REM 编译项目
echo 编译项目...
dotnet build -c Release -o bin
if %errorlevel% neq 0 (
    echo 编译失败
    pause
    exit /b 1
)

REM 复制配置文件
echo 复制配置文件...
copy config.json bin\config.json >nul 2>&1

echo 编译完成！
echo 生成的插件文件在 bin 目录中
echo 请将 bin 目录中的所有文件复制到 csgo/addons/counterstrikesharp/plugins/SpawnPorter/ 目录
pause 