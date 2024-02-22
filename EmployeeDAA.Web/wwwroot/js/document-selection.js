var docName = null;
$(document).ready(function() {
  $(window).on('load', function(){
    $('.row--select-checkbox input[type="checkbox"]').on('change', function(){
      $('.doc-name').text($(this).closest('tr').find('.task a').text())
      if(countSelection() == 1) {
        $('.doc-name').text($('.row--select-checkbox input[type="checkbox"]:checked').closest('tr').find('.task a').text());
      }
    });
  });
});


function countSelection() {
  var totalSelected = $('.row--select-checkbox input[type="checkbox"]:checked').length;
  $('.total-selected').text(totalSelected)
  toggleAction(totalSelected)
  return parseInt(totalSelected);
  
}


function toggleAction(no) {
    if(no == 1) {
      $('.btn-restore').show()
      $('.btn-remove-star').show()
      $('.btn-view').show()
      $('.btn-print').show()
      $('.btn-move').show()
      $('.clissify-files').show()
      $('.multi__folder--selected').hide()
      $('.multi__file--selected').hide()
      $('.single__folder--selected').show()
      $('.single__file--selected').show()
      $('.no__item_selected').hide()
    } else {
      $('.btn-view').hide()
      $('.btn-print').hide()
      $('.btn-restore').hide()
      $('.btn-remove-star').hide()
    }
    if(no >= 1) {
        $('.btn-download').show()
        $('.btn-Pdf-download').show()
      $('.btn-copy').show()
      $('.btn-delete').show()
      $('.btn-restore').show()
      $('.btn-remove-star').show()
      $('.btn-view').show()
      $('.btn-print').show()
    } else {
      $('.btn-move').hide()
      $('.clissify-files').hide()
      $('.single__folder--selected').hide()
      $('.single__file--selected').hide()
     
        $('.btn-download').hide()
        $('.btn-Pdf-download').hide()
        
      $('.btn-copy').hide()
      $('.btn-delete').hide()
      $('.no__item_selected').show()
      console.log('call')
    }
    if(no > 1) {
      $('.btn-move').show()
      $('.clissify-files').hide()
      $('.multi__folder--selected').show()
      $('.multi__file--selected').show()
      $('.single__folder--selected').hide()
      $('.single__file--selected').hide()
      $('.no__item_selected').hide()
    }
    if(no == 2) {
      $('.btn-compare').show()
    } else {
      $('.btn-compare').hide()
    }
}