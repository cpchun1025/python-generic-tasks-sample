import os
import shutil
import datetime

def move_old_files(src_folder, archive_folder, days=7):
    # Ensure the archive folder exists
    os.makedirs(archive_folder, exist_ok=True)

    # Get the current date
    now = datetime.datetime.now()

    # Iterate over files in the source folder
    for file_name in os.listdir(src_folder):
        file_path = os.path.join(src_folder, file_name)

        # Check if it's a file (not a directory)
        if os.path.isfile(file_path):
            # Get the file's creation time
            creation_time = datetime.datetime.fromtimestamp(os.path.getctime(file_path))

            # Check if the file is older than `days`
            if (now - creation_time).days > days:
                # Move the file to the archive folder
                shutil.move(file_path, os.path.join(archive_folder, file_name))
                print(f"Moved: {file_path} -> {archive_folder}")

if __name__ == "__main__":
    src_folder = "C:\\path\\to\\source\\folder"
    archive_folder = "C:\\path\\to\\archive\\folder"
    move_old_files(src_folder, archive_folder, days=7)