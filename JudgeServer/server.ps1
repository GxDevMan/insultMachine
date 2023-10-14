# Activate the virtual environment
& "aiInferencing\Scripts\Activate"

# Change the working directory
Set-Location "cnnsvmServer"

# Start the Django server
python manage.py runserver
