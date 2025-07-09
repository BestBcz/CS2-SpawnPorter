#!/bin/bash

echo "正在编译 SpawnPorter CS Sharp 插件..."

# 检查是否存在 .NET SDK
if ! command -v dotnet &> /dev/null; then
    echo "错误: 未找到 .NET SDK"
    echo "请确保已安装 .NET 8.0 或更高版本"
    exit 1
fi

# 创建输出目录
mkdir -p bin

# 编译项目
echo "编译项目..."
dotnet build -c Release -o bin
if [ $? -ne 0 ]; then
    echo "编译失败"
    exit 1
fi

# 复制配置文件
echo "复制配置文件..."
cp config.json bin/config.json 2>/dev/null || true

echo "编译完成！"
echo "生成的插件文件在 bin 目录中"
echo "请将 bin 目录中的所有文件复制到 csgo/addons/counterstrikesharp/plugins/SpawnPorter/ 目录" 