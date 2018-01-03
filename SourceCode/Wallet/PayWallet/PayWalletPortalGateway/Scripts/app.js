$('.slide').owlCarousel({
    loop:true,
    autoplay:200,
    margin:0,
    nav:false,
    autoWidth:true,
    responsive:{
        0:{
            items:1
        },
        600:{
            items:1
        },
        1000:{
            items:1
        }
    }
});

        if($(window).width() <= 1139){
            var $window = $(window);
            var $rzObj = $("#layers-wrapper");
            var ratio;
                _wh = $window.height();
                _ww = $window.width();
                ratio = _ww/1140;
                $rzObj.css({
                    "transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")",
                    "-webkit-transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")",
                    "-moz-transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")",
                    "-o-transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")",
                    "-ms-transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")"
                });


        }
    $(window).resize(function(){
        if($(window).width() <= 1139){
            var $window = $(window);
            var $rzObj = $("#layers-wrapper");
            var ratio;
                _wh = $window.height();
                _ww = $window.width();
                ratio = _ww/1140;
                $rzObj.css({
                    "transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")",
                    "-webkit-transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")",
                    "-moz-transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")",
                    "-o-transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")",
                    "-ms-transform": "perspective(1px) scale(" + ratio + ", " + ratio + ")"
                });


        }




    });
