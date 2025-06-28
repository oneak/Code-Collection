"""
    Random Byte Generator
    Version: 1.0
    License: GNU GPL v2
    Author: Oneak (https://realmmadness.com/oneak)

    This script generates a specified number of random numeric "bytes". 
    In this context, "bytes" refers to individual digits (0-9) that are generated 
    to form a random numeric string of the specified length. The script prompts the 
    user for how many digits (bytes) they want to generate, then creates and displays 
    the result as a string of random numeric characters.

    Licensed under the GNU General Public License v2.
"""

import random
import msvcrt

# Display script title and version
print("=" * 40)
print(" Random Byte Generator v1.0 ")
print(" Oneak RealmMadness.com ")
print("=" * 40)

# Inform user about the purpose of the script
print("This script generates a random numeric string (byte sequence).")
print("Each 'byte' is represented by a single random digit (0-9).")
print("=" * 40)

while True:
    # Get user input for the number of bytes (digits) to generate
    user_input = input("Enter the number of bytes to generate (or type 'exit' to quit): ")

    if user_input.lower() == 'exit':
        break

    # Validate input
    try:
        num_bytes = int(user_input)
        if num_bytes > 0:
            # Generate random bytes as numeric digits
            key = ''.join(random.choices('0123456789', k=num_bytes))

            # Display the generated random numeric key
            print("\nGenerated Key:", key)
        else:
            print("Please enter a positive number of bytes.")
    except ValueError:
        print("Invalid input. Please enter a valid number.")

# Pause like cmd (wait for user input before closing the script)
print("\nPress any key to exit...")
msvcrt.getch()