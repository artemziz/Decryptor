let imagesCase = document.getElementsByClassName("cat");
let images = [];
console.log("eqwe");
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

},500);