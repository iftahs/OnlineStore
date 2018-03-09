function isDate(txtDate)
{
    var currVal = txtDate;
    if (currVal == '')
        return false;
    //Declare Regex
    
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;
    //Checks for mm/dd/yyyy format.

    dtMonth = dtArray[3];
    dtDay = dtArray[1];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2)
    {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}



var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

//script for registration form
$("#RegistrationForm").submit(function (e) {
    e.preventDefault();

    var firstName = $(this).find("#txtFirstName");
    var lastName = $(this).find("#txtLastName");
    var email = $(this).find("#txtEmail");
    var password = $(this).find("#txtPassword");
    var city = $(this).find("#txtCity");
    var street = $(this).find("#txtSteet");
    var houseNumber = $(this).find("#txtHouseNumber");
    var zipCode = $(this).find("#txtZipCode");
    var birthDate = $(this).find("#txtBirthDate");

    var valid = true;
    if (firstName.val() == "") {
        valid = false;
        firstName.addClass("NotValid");
    }
    else {
        firstName.removeClass("NotValid");
    }

    if (lastName.val() == "") {
        valid = false;
        lastName.addClass("NotValid");
    }
    else {
        lastName.removeClass("NotValid");
    }

    if (email.val() == "") {
        valid = false;
        email.addClass("NotValid");
    }
    else {
        if (!re.test(email.val())) {
            valid = false;
            email.addClass("NotValid");
        }
        else {
            email.removeClass("NotValid");
        }
    }

    if (birthDate.val() == "") {
        valid = false;
        birthDate.addClass("NotValid");
    }
    else {
        if (isDate(birthDate.val())) {
            valid = false;
            birthDate.addClass("NotValid");
        }
        else {
            birthDate.removeClass("NotValid");
        } 
    }

    if (password.val() == "" || password.val().length < 8) {
        valid = false;
        password.addClass("NotValid");
    }
    else {
        password.removeClass("NotValid");
    }

    if (valid == false) {
        $("#Message").show();
    }
    else {
        $("#btnSubmit").attr("disabled", true);
        $("#Message").show();
        $("#Message").text("Please wait...");

        data = {
            FirstName: firstName.val(),
            LastName: lastName.val(),
            Email: email.val(),
            Password: password.val(),
            City: city.val(),
            Street: street.val(),
            HouseNumber: houseNumber.val(),
            ZipCode: zipCode.val(),
            BirthDate: birthDate.val()
        }

        function success() {
            $("#Message").text("Registration success");
            $("#Message").removeClass("alert-danger");
            $("#Message").addClass("alert-success");
            $("#RegistrationForm").hide();
        }

        $.post("/Api/UserActions/Registration", data, success)
            .fail(function fail(jqXHR, textStatus, errorThrown) {
                var responseText = jQuery.parseJSON(jqXHR.responseText);
                $("#Message").text("Registration fail: " + responseText.Message);
                $("#Message").removeClass("alert-success");
                $("#Message").addClass("alert-danger");
                $("#btnSubmit").attr("disabled", false);
            });  
    }

});

//script for login form
$("#frmLogin").submit(function (e) {
    e.preventDefault();
    var email = $(this).find("#txtEmail");
    var password = $(this).find("#txtPassword");

    var valid = true;

    //Check email field
    if (email.val() == "") {
        valid = false;
        email.addClass("NotValid");
    }
    else {
        if (!re.test(email.val())) {
            valid = false;
            email.addClass("NotValid");
        }
        else {
            email.removeClass("NotValid");
        }
    }

    //Check email field
    if (password.val() == "" || password.val().length < 8) {
        valid = false;
        password.addClass("NotValid");
    }
    else {
        password.removeClass("NotValid");
    }

    if (valid == false) {
        $("#Message").show();
    }
    else {
        $("#btnSubmit").attr("disabled", true);
        $("#Message").show();
        $("#Message").text("Please wait...");

        function success() {
            window.location.href = "/User/MyDetails";
        }

        $.post("/Api/UserActions/Login", { Email: email.val(), Password: password.val() }, success)
            .fail(function (jqXHR, testStatus, errorThrown) {
                var responseText = jQuery.parseJSON(jqXHR.responseText);
                $("#Message").text("Login fail: " + responseText.Message);
                $("#Message").removeClass("alert-success");
                $("#Message").addClass("alert-danger");
                $("#btnSubmit").attr("disabled", false);
            });
    }
});

function GetUserFirstName() {
    $("#userNav").html("Please wait...");
    $.get("/Api/UserActions/GetUserFromCookie", null, function (user) {
        $("#userNav").html("Hello " + user.FirstName + " (<a href='#' onclick='logout();'>Logout</a>)");
    }).fail(function () {
        $("#userNav").html("Hello Guest (<a href='/User/Login'>Login</a>)");
    });
}

function logout() {
    $.post("/Api/UserActions/Logout", null, function () {
        $("#userNav").html("Hello Guest (<a href='/User/Login'>Login</a>)");
        window.location.href = "/";
    });
}

function getCategories() {
    var categoriesInRow = 4;
    $.get("/Api/CategoryActions/GetAllCategories", null, function (categories) {
        if (categories.length == 0) {
            $("#tblCategories").append("<tr><td style='text-align:center; font-size:3em;'>No categories found</td></tr>")
        }
        else {
            for (i = 0; i < categories.length; i++) {
                if (i % categoriesInRow == 0)
                    $("#tblCategories").append("<tr></tr>")
                $("#tblCategories tr:last-child").append("<td><a href='/Category/ShowCategory/" + categories[i].CatId + "'>" + categories[i].CatName + "</a></td>");

            }
        }
    });
}

function fixCardsHeight() {
    var max = 0;
    $(".card").each(function () {
        if ($(this).height() > max)
            max = $(this).height();
    });

    $(".card").each(function () {
        $(this).height(max);
    });
}

function getCartCount() {
    $.get("/Api/OrderActions/GetCartAmount", null, function (amount) {
        $("#cartAmount").text(amount);
        if (amount == 0) {
            $("#btnPlaceOrder").prop('disabled', true);
            $("#btnEmptyCart").prop('disabled', true);
        }
        else {
            $("#btnPlaceOrder").prop('disabled', false);
            $("#btnEmptyCart").prop('disabled', false);
        }
    });
}

function addToCart(id) {
    $.post("/Api/OrderActions/AddToCart", { ProductId: id }, function () {
        alert("Product added to shopping cart successfully!");
        getCartCount();
    })
        .fail(function (jqXHR, testStatus, errorThrown) {
            var responseText = jQuery.parseJSON(jqXHR.responseText);
            if (errorThrown == "Unauthorized")
                alert("You need to login first!");
            else
                alert("Error on add to cart: " + testStatus + " | " + errorThrown + " | " + responseText.Message);
        });
}

function getShoppingCartItems() {
    $.get("/Api/OrderActions/GetCartProducts", null, function (items) {
        if (items.length == 0) {
            $("#tblShoppingCart").after("<h2>Cart is Empty</h2>");
            $("#tblShoppingCart").hide();
            $("#productAmount").html(0);
            $("#totalPrice").html(0);
        }
        else {
            var total = 0;
            var amount = 0;
            for (i = 0; i < items.length; i++) {
                total += items[i].Total;
                amount += items[i].Amount;
                var row = "<tr>" +
                    "<td><button class='btn btn-danger' onclick='removeItem(" + items[i].ProductId + ");'>X</button></td>" +
                    "<td>" + items[i].ProductName + "</td>" +
                    "<td>" + items[i].Price + "</td>" +
                    "<td>" + items[i].Amount + "</td>" +
                    "<td>" + (items[i].Total).toFixed(2) + "</td>" +
                    "</tr>";
                $("#tblShoppingCart tbody").append(row);
            }
            $("#productAmount").html(amount);
            $("#totalPrice").html(total.toFixed(2));
        }
    });
}

function removeItem(id) {
    if (!$.isNumeric(id))
        alert("No item selected!");
    else {
        $.post("/Api/OrderActions/RemoveItemFromCart", { ProductId: id }, function () {
            $("#tblShoppingCart tbody tr").remove();
            getShoppingCartItems();
        }).fail(function () {
            alert("Fail on remove item from cart.");
        });
    }
}

function finishOrder() {
    $("#btnPlaceOrder").prop('disabled', true);
    $("#btnPlaceOrder").text("Please wait...");
    $.post("/Api/OrderActions/MakeOrder", null, function () {
        alert("Order placed successfully!");
        window.location.href = "/Order/ShowOrders"
    }).fail(function () {
        $("#btnPlaceOrder").prop('disabled', false);
        $("#btnPlaceOrder").text("Confirm and pay");
        alert("Error when trying to place order.");
    });
}

function emptyCart() {
    var conf = confirm("Are you sure you want to empty the cart?");
    if (conf) {
        $.post("/Api/OrderActions/EmptyCart", null, function () {
            getCartCount();
            getShoppingCartItems();
        }).fail(function () {
            alert("Error on empty cart.");
        })
    }
}

$("#frmAddCategory").submit(function (e) {
    e.preventDefault();
    var catName = $(this).find("#txtCatName");
    if (catName.val() == "") {
        catName.addClass("NotValid");
        alert("Category name is mandatory!");
    }
    else {
        catName.removeClass("NotValid");
        $.post("/Api/CategoryActions/AddCategory", { CatName: catName.val() }, function () {
            catName.val("");
            $("#tblCategories tr").remove();
            getCategories();
        }).fail(function () {
            alert("Error on add category!");
        });
    }
}); 

$("#frmAddProduct").submit(function (e) {
    e.preventDefault();
    var ProductName = $(this).find("#txtProductName");
    var CatId = $(this).find("#categorySelect");
    var ShortDescription = $(this).find("#txtShortDescription");
    var FullDescription = $(this).find("#txtFullDescription");
    var Price = $(this).find("#txtPrice");
    var valid = true;

    if (ProductName.val() == "") {
        ProductName.addClass("NotValid");
        valid = false;
    }
    else {
        ProductName.removeClass("NotValid");
    }

    if (ShortDescription.val() == "") {
        ShortDescription.addClass("NotValid");
        valid = false;
    }
    else {
        ShortDescription.removeClass("NotValid");
    }

    if (Price.val() == "") {
        Price.addClass("NotValid");
        valid = false;
    }
    else {
        if ($.isNumeric(Price.val()))
            Price.removeClass("NotValid");
        else {
            Price.addClass("NotValid");
            valid = false;
        }
    }

    if (valid == false) {
        $("#Message").show();
    }
    else {
        $("#btnSubmit").attr("disabled", true);
        $("#Message").show();
        $("#Message").text("Please wait...");

        data = {
            ProductName: ProductName.val(),
            CatId: CatId.val(),
            ShortDescription: ShortDescription.val(),
            FullDescription: FullDescription.val(),
            Price: Price.val()
        };

        $.post("/Api/ProductActions/AddProduct", data, function () {
            alert("Product added successfully!");
            $("#frmAddProduct")[0].reset();
            $("#Message").hide();
        }).fail(function () {
            alert("Error on adding product.");
        });
    }

});

$(document).ready(function () {
    //alert(window.location.pathname);
    GetUserFirstName();
    getCategories();
    fixCardsHeight();
    getCartCount();
    getShoppingCartItems();
});