#!/bin/bash

function CheckOS() {
    UNAME=$(uname)
    if [[ "$UNAME" == CYGWIN* || "$UNAME" == MINGW* ]]; then
        is_windows=0
    fi
}

CheckOS

global_json_file="$(dirname "$0")/../global.json"

dotnet_install_dir="$(dirname "$0")/../.dotnet"

if [ $is_windows ]; then
    install_script_url="https://dot.net/v1/dotnet-install.ps1"
    install_script="$(dirname "$0")/dotnet-install.ps1"
else
    install_script_url="https://dot.net/v1/dotnet-install.sh"
    install_script="$(dirname "$0")/dotnet-install.sh"
fi

echo "Downloading '$install_script_url'"
curl "$install_script_url" -sSL --retry 10 --create-dirs -o "$install_script"

if [ $is_windows ]; then
    powershell -executionpolicy bypass $(dirname "$0")/dotnet-install.ps1 -installDir $(dirname "$0")/../.dotnet -JSonFile "$global_json_file"
else
    chmod +x "$install_script"
    "$install_script" --install-dir $(dirname "$0")/../.dotnet --jsonfile $global_json_file
fi
