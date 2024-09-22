@echo off
REM Activate the Conda environment
call conda activate <your_env_name>

REM Run the Python script in unbuffered mode to get real-time output
python -u <your_script.py>

REM Deactivate the environment after execution
call conda deactivate