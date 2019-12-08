let imagesCase = document.getElementsByClassName("cat");
let images = [];

for(let i =0;i<imagesCase.length;i++){
    images.push(imagesCase[i].firstElementChild);
}

setTimeout(function(){

    for(let i = 0;i<images.length;i++){
    images[i].style.width = 300+"px";
    images[i].style.height = 300+"px";
}
setTimeout(()=>{
    document.getElementById("logo").style.visibility = "visible";
},1800);

}, 500);



let errors = document.body.getElementsByClassName("errors")[0];
//if (errors.getElementsByClassName("mdl-cell mdl-cell--12-col").length != 0) {
//    errors.getElementsByClassName("mdl-cell mdl-cell--12-col")[0].style.visibility = 'visible';
//}



