import time
import sys

sys.stdout.flush()  # Force Python to flush output after every print statement

count = 0

while True:
    print(f"Count: {count}")
    count += 1
    time.sleep(1)  # Wait for 1 second before incrementing the count
