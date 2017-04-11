$(document).ready(function () {
    $.ajax(
  {
      url: '/Contractor/_Pending',
      data: { Id: "Main" }, //input parameters to action
      beforeSend: function () {
          $('#div-result').show();
          var img = '<img src="../../Images/loading.ico" />';
          $('#div-result').html(img);
      },
      success: function (data) {
          $('#Main').html(data);
      }
  });
});