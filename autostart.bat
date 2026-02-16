@echo off
TITLE Avvio Docker Compose

echo Verifica installazione Docker in corso...

where docker >nul 2>nul

if %errorlevel% neq 0 (
    cls
    color 0C
    echo ========================================================
    echo  ERRORE: Docker non e' installato o non e' nel PATH!
    echo ========================================================
    echo.
    echo Per favore installa Docker Desktop per Windows.
    echo.
    pause
    exit /b
)

color 0A
echo Docker trovato! Avvio dei container...
echo.

docker compose up -d --build

echo.
if %errorlevel% equ 0 (
    echo Fatto! I container sono stati avviati correttamente.
) else (
    color 0C
    echo Si e' verificato un errore durante l'avvio (Docker Desktop e' aperto?).
)

pause
