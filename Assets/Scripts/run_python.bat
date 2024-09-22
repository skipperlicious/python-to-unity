@echo off
REM Directly run the Python script without activating Conda

REM Run the Python script in unbuffered mode to get real-time output
python -u "%~dp0\Python Scripts\test_counter.py"
