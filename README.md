# MovieLibApp
**MovieLibApp** is a school project for the module **Device Programming** of my study at **MCT Howest (Kortrijk)**. This project is an android app to save your favorite movies and add a review to a movie.

### How does it work?

When opening the app, you can view a list of all your favorite movies, popular movies or search a movie using the search bar. You can select any movie for more details, to save it as your favorite or add a review.

You can find a **[demo video](https://github.com/DumortierJens/MovieLibApp/blob/master/movielibapp.mp4 "movielibapp.mp4")** in the root folder.

### What dit I use?

* For creating the app, I used **Xamarin Forms**

* For getting the movies and save it as my favorite, I used the **TheMovieDB API**:  https://developers.themoviedb.org/3

* For saving the users review I created an API with **Azure funtions** and saved the reviews in **Azure storage tables**

* For showing the rating in stars, I used a Nuget Package from **Syncfusion**: https://help.syncfusion.com/xamarin/rating/getting-started

### How to get started

1. Setup **Azure functions** with the functions app  [MovieLibAPI](https://github.com/DumortierJens/MovieLibApp/tree/master/MovieLibApp/MovieLibAPI "MovieLibAPI") 

2. Create an **Azure storage account** and save the connectionstring in the functions app

3. Create a **TheMovieDB API key and session**: https://www.themoviedb.org/settings/api

4. Get a **Syncfusion license**: https://www.syncfusion.com/products/communitylicense

5. Add **User Secrets** to your project with the syncfusion licensing key, TheMovieDB api key and TheMovieDB session Id
 {
	  "SyncfusionLicensing": "",
	  "APIKEY": "",
	  "SESSIONID": ""
}

6. **Build** the app and **run** it on your phone
