function showSuccessmsg() {

    $('#divSuccess').show();
    setTimeout(hideSuccessmsg, 5000);
}
function hideSuccessmsg(Id) {

    $('#divSuccess').hide();
}


// MM Menu 
//$(document).ready(function () {
//    // executes when complete page is fully loaded, including all frames, objects and images
//    //alert("window is loaded");
//    $('.box').matchHeight();
//});

//Slick Slider 
//$('.single-item').slick();


$('.single-item').slick({
    dots: false,
    infinite: false,
    speed: 300,
    responsive: [
      {
          breakpoint: 480,
          settings: {
              dots: false,
              infinite: true,
              autoplay: true,
              arrows: false,
              slidesToShow: 1,
              slidesToScroll: 1
          }
      }
       //You can unslick at a given breakpoint now by adding:
       //settings: "unslick"
       //instead of a settings object
    ]
});