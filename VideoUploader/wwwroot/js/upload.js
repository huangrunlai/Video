$(document).ready(function () {
    $('#uploadForm').submit(function (event) {
        event.preventDefault();
        var formData = new FormData(this);

        $.ajax({
            url: '/api/upload',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function () {
                $('#uploadStatus').text('Upload successful!');
                window.location.href = '/'; // Redirect to Catalog page
            },
            error: function (xhr, status, error) {
                $('#uploadStatus').text('An error occurred: ' + error);
            }
        });
    });
});
