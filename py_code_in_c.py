import sys
import pandas as pd

def generate_excel(input_file, output_file):
    # Read input (e.g., parsed email data)
    with open(input_file, 'r') as f:
        data = f.readlines()

    # Example: Generate Excel
    df = pd.DataFrame({'EmailContent': data})
    df.to_excel(output_file, index=False)

if __name__ == "__main__":
    input_file = sys.argv[1]
    output_file = sys.argv[2]
    generate_excel(input_file, output_file)
