


//Template Name: Velzon - Admin & Dashboard Template
//Author: Themesbrand
//Website: https://Themesbrand.com/
//Contact: Themesbrand @gmail.com
//File: Form wizard Js File
  

// user profile img file upload
if (document.querySelector("#profile-img-file-input"))
    document.querySelector("#profile-img-file-input").addEventListener("change", function () {
        var preview = document.querySelector(".user-profile-image");
        var file = document.querySelector(".profile-img-file-input").files[0];
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            preview.src = reader.result;
        }, false);

        if (file)
            reader.readAsDataURL(file);
    });
    