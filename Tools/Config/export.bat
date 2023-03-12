

tabtoy.exe ^
--mode=v2 ^
--csharp_out=.\exports\Config.cs ^
--binary_out=.\exports\config.bin ^
--json_out=.\exports\config.json ^
--combinename=Config ^
--lan=zh_cn ^
tables\GoodConfig.xlsx ^
tables\LanguageData.xlsx ^
tables\Tower.xlsx
copy ".\exports\config.bin" ..\RLS_Project\Assets\StreamingAssets\Configs\config.bin
copy ".\exports\Config.cs" ..\RLS_Project\Assets\Scripts\Config\Config.cs
pause
@IF %ERRORLEVEL% NEQ 0 pause