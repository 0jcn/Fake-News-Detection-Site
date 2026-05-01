# Fake News Detection Site
Fake News detection site, being built using React for the frontend, ASP.NET API as the middle layer for connecting to the database and to the Python API for Machine Learning operations. 
# Aims of this project
The main aim of this project was to a create a machine learning model to efficiently and accurately detect political fake news.
# Features
## Site
- User Accounts
- Users can save detections
- Users can view their own previously saved detections
## API
- Connect to the database
- Connect to the Python API for machine learning operations
- Act as a middle layer between the site and the db + machine learning model
## Machine Learning
- Be able to classify/ score articles based on likeliness to be fake news
- Achieves an 84.8% accuracy score with 0.85 macro and weighted F1 scores
- The model and vectorizer can be found at this link: https://drive.proton.me/urls/KDF9W6BMD8#BzWJN0sPJLF2 due to GitHub's file sizes
# Running the application
## Requirements
- Docker installed on local machine
- Repository
- The model and vectorizer from the above link
## Running
Docker compose has been setup within the application to be able to easily run the project
To run the application, place the model and vectorizer with the ```Python/Fake-News-Detector/``` directory
After this navigate back to the base directory of the repository and open in terminal
Ensure docker is running and run the ```docker compose up --build -d``` command this will create the images for each of the services then build the containers
Once this process has completed, the frontend will be avaliable at http://localhost:3000/ and the whole site will be avaliable for use
