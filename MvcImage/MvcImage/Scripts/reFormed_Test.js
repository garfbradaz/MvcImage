(function ($) {

    $.fn.reFormed = function (options) {

        var settings = {
            reFormedOffClass: '.reFormedOff',          /*CSS class of element to turn off the modal form*/
            reFormedOnClass: '.reFormedOn',
            mask: 'backGroundMask',
			reForm: '.modal_window'
        };
		
		//Get the height/width of the page.
		var windowWidth = $(window).width();
		var windowHeight = $(window).height();
        return this.each(function () {

            if (options) {
                $.extend(settings, options);
            }
			var $this = $(this); //stain
			
			
			var modalHeight = $(settings['reForm']).outerHeight();
			var modalWidth = $(settings['reForm']).outerWidth();
			
			var top = (windowHeight-modalHeight)/2;  
			var left = (windowWidth-modalWidth)/2; 
			
			//apply new top and left css values  
			$(settings['reForm']).css({'top' : top , 'left' : left}); 
			
			$(settings['reFormedOnClass']).click(function () {

				//get the id of the modal window stored in the name of the activating element  
				var modalId = $(this).attr('name');

				//use the function to show it  
				showModalForm(settings['mask'],modalId);

			});
				

			$(settings['reFormedOffClass']).click(function () {
				var modalId = $(this).attr('name');
				closeModalForm(settings['mask']);
			});
		});

		
    };
})(jQuery);

function showModalForm(maskId,modalId) {
    //set the backgorund.
    $('#'+maskId).css({ 'display': 'block', opacity: 0 });

    //fade in the mask to opacity 0.8  
    $('#'+maskId).fadeTo(500, 0.8);
	
	//Display the modal form
	$('#'+modalId).fadeIn(500);
}

function closeModalForm(maskId,modalId) {

    //Hide the background Mask and Form
    //$('#'+maskId).fadeOut(1000);
	
	$('#'+maskId).fadeOut(1000);
	$('#'+modalId).hide();
	//$('#'+modalId).css({ 'display': 'none', opacity: 0 });
	//$('#'+maskId).css({ 'display': 'none', opacity: 0 });
}

