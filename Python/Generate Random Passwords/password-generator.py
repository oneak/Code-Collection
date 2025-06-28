"""
    Password Generator
    Version: 1.0
    License: GNU GPL v2
    Author: Oneak (https://realmmadness.com/oneak)

    This script generates a specified number of random passwords.
    Licensed under the GNU General Public License v2.
"""

import random
import string

# Display script title and version
print("=" * 40)
print(" Password Generator ")
print(" Oneak RealmMadness.com ")
print("=" * 40)
print(" Generate random passwords")
print("=" * 40)

while True:
    # Ask for the password length or to exit
    length_input = input("Enter the length of the password (or type 'exit' to quit): ")

    if length_input.lower() == 'exit':
        print("\nExiting the script. Goodbye!")
        break

    # Validate length input
    try:
        length = int(length_input)
        if length < 1:
            length = 1
        elif length > 5000:
            length = 5000
    except ValueError:
        print("Invalid input. Please enter a valid number.")
        continue

    # Define the characters set for password generation
    chars = string.ascii_letters + string.digits + string.punctuation

    # Generate the password
    password = ''.join(random.choice(chars) for _ in range(length))

    # Display the generated password
    print("\nYour password is:", password)
    print("=" * 40)