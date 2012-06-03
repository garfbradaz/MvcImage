$(document).ready(function () {
    var inputTag = ".upload";
    var imgTag = "#imgThumbnail2";
    var wantThumbnail = $("#thumbnail").length;
    if (wantThumbnail > 0) {
        var imgTTag = "#thumbnail";
    }
    $(inputTag).change(function () {

        //-- Create a input attribute with the same type as the original
        //-- Insert it after the orignals location.
        $('<input>').attr(
            {
                class: "upload-clone",
                type: $(inputTag).attr('type'),
                name: "name-clone"

            }).insertAfter(inputTag);

        //--Create a hidden form with an action method pointer to
        //--our custom controller. 
        $("<form>").attr(
			{
			    method: "post",
			    id: "prototype",
			    action: "/Image/AjaxSubmit/" + wantThumbnail

			}).appendTo("body").hide();

        //--Change the encoding based on the browser, as IE doent allow you to change the encoding.
        //--Append our orignal input to the hidden form.
        $('#prototype').attr((this.encoding ? 'encoding' : 'enctype'), 'multipart/form-data');

        //$(inputTag).hide();
        $(inputTag).appendTo("#prototype").hide();


        //--Use AJAX to post the form, and if successful, load the binary info the image tag.
        $('#prototype').ajaxSubmit(
                {
                    success: function (responseText) {
                        var d = new Date();

                        $(imgTag)[0].src = "/Image/ImageLoad/0";

                        if (wantThumbnail > 0) {
                            $(imgTTag)[0].src = "/Image/ImageLoad/" + wantThumbnail;
                        }

                        $(inputTag).insertAfter('.upload-clone').show();
                        $('.upload-clone').remove();
                        $('#prototype').remove();
                    }

                });

        /*
				
        $('upload-clone').appendTo('#prototype');
        $('#prototype').remove();
				
        */

    });
});

