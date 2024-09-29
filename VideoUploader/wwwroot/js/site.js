function VideoViewModel() {
    var self = this;
    self.videos = ko.observableArray([]);

    // Play selected video
    self.playVideo = function (video) {
        var videoPlayer = document.getElementById('videoPlayer');
        videoPlayer.src = '/media/' + video.fileName;
        videoPlayer.load();
        videoPlayer.play();
        videoPlayer.hidden = false;
    };

    // Load videos from server
    self.loadVideos = function () {
        $.ajax({
            url: '/api/video',
            type: 'GET',
            success: function (data) {
                if (data && data.length > 0) {
                    self.videos(data);  // Populate videos if available
                } else {
                    self.videos([]); // Empty array to indicate no videos
                }
            },
            error: function () {
                console.error('Failed to load videos');
            }
        });
    };

    self.loadVideos(); // Initial load when the page is loaded
}
ko.applyBindings(new VideoViewModel());
