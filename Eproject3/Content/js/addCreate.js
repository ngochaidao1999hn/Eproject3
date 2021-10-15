$("#addContent").click(function e() {
    $("<p id=\"cont\"> <input type=\"text\" id=\"content\" name=\"txtText\" required />" + " <input type=\"file\" name=\"Url\" /></p>").insertBefore("#addContent");
});
$("#RemoveContent").click(function e() {
    $("#RemoveContent").prev().prev().remove();
});
$("#addIgre").click(function e() {
    $("<p id=\"igre\"> <input type=\"text\" id=\"content\" name=\"txtIgredent\" /></p>").insertBefore("#addIgre");
});
$("#RemoveIgre").click(function e() {
    $("#RemoveIgre").prev().prev().remove();
});