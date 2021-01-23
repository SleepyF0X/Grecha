/* Set the width of the side navigation to 250px */
function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    document.getElementById("bg-blackout").style.display = "block";
    $("#bg-blackout").animate({ opacity: 0.4 }, 500);
}

/* Set the width of the side navigation to 0 */
function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    $("#bg-blackout").animate({ opacity: 0 }, 500, function() {
        document.getElementById("bg-blackout").style.display = "none";
    });
}