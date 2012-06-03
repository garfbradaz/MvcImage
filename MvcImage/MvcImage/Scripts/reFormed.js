(function ($) {

    $.fn.reFormed = function (options) {

        var settings = {
            reFormedOffClass: '.reFormedOff',          /*CSS class of element to turn off the modal form*/
            reFormedOnClass: '.reFormedOn',
            mask: 'backGroundMask'
        };
		
		//Get the height/width of the page.
		var windowWidth = $(window).width();
		var windowHeight = $(window).height();
        return this.each(function () {

            if (options) {
                $.extend(settings, options);
            }


			var $this = $(this); //stain
			
			$(settings['reFormedOnClass']).click(function () {

				//get the id of the modal window stored in the name of the activating element  
				var modalId = $(this).attr('name');

				//use the function to show it  
				showModalForm(settings['mask']);

			});
				

			$(settings['reFormedOffClass']).click(function () {

				closeModalForm(settings['mask']);
			});
		});

		
    };
})(jQuery);

function showModalForm(modalId) {
    //set the backgorund.
    $('#'+modalId).css({ 'display': 'block', opacity: 0 });

    //fade in the mask to opacity 0.8  
    $('#'+modalId).fadeTo(500, 0.8);
}

function closeModalForm(modalId) {

    //Hide the background Mask
    $('#'+modalId).fadeOut(500)
}

