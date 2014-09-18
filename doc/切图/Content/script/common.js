$(function() {
	$('.banner').unslider({
		speed: 500,               //  The speed to animate each slide (in milliseconds)
		delay: 3000,              //  The delay between slide animations (in milliseconds)
		complete: function() {},  //  A function that gets called after every slide animation
		keys: true,               //  Enable keyboard (left, right) arrow shortcuts
		dots: true,               //  Display dot navigation
		fluid: false              //  Support responsive design. May break non-responsive designs
	});
	
	jwplayer('video-box').setup({
		skin:"/static/script/five.xml",
        width:1000,
        height:530,
        playlist: [{
            image: "/static/image/video/video.png",
            sources: [{
                file: "/static/image/video/360p.mp4",
                label: "360p",
                //"default": true
            },{
                file: "/static/image/video/720p.mp4",
                label: "720p"
            },{
                file: "/static/image/video/1080p.mp4",
                label: "1080p"
            }]
        }],
        primary: "flash",
        autostart:true,
        startparam: "start",
        autochange:true,
	});
});