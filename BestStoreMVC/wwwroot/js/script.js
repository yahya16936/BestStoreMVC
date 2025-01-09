﻿function getShoppingCart() {
    // document.cookie contains all the cookies of our website with the following format
    // cookie1=value1; cookie2=value2; cookie3=value3;

    const cookieName = "shopping_cart";
    let cookiesArray = document.cookie.split(';');

    for (let i = 0; i < cookiesArray.length; i++) {
        let cookie = cookiesArray[i];
        if (cookie.includes(cookieName)) {
            let cookie_value = cookie.substring(cookie.indexOf("=") + 1);

            try {
                let cart = JSON.parse(atob(cookie_value));
                return cart;
            }
            catch (exception) {
                break;
            }
        }
    }

    return {};
}


function saveCart(cart) {
    let cartStr = btoa(JSON.stringify(cart))

    // save cookie
    let d = new Date();
    d.setDate(d.getDate() + 365); // this cookie expires after 365 days (1 year)
    let expires = d.toUTCString();
    document.cookie = "shopping_cart=" + cartStr + ";expires=" + expires + ";path=/; SameSite=Strict; Secure";
}



function addToCart(button, id) {
    let cart = getShoppingCart();

    let quantity = cart[id]
    if (isNaN(quantity)) {
        // quantity is Not a Number => set quantity to 1
        cart[id] = 1
    }
    else {
        cart[id] = Number(quantity) + 1;
    }

    saveCart(cart);
    button.innerHTML = "Added <i class='bi bi-check-lg'></i>";
    let cartSize = 0;
    for (var cartItem of Object.entries(cart)) {
        quantity = cartItem[1]
        if (isNaN(quantity)) continue;

        cartSize += Number(quantity)
    }

    document.getElementById("CartSize").innerHTML = cartSize
}

function increase(id) {
    let cart = getShoppingCart();
    let quantity = cart[id]
    if (isNaN(quantity)) {
        //quantity is Not a Number => set it to 1
        cart[id] = 1
    }
    else {
        cart[id] = Number(quantity) + 1;
    }

    saveCart(cart);
    location.reload()
}

function decrease(id) {
    let cart = getShoppingCart();
    let quantity = cart[id]
    if (isNaN(quantity)) {
        //quantity is Not a Number => exit
        return
    }

    quantity = Number(quantity)

    if (quantity > 1) {
        cart[id] = Number(quantity) - 1;
        saveCart(cart);
        location.reload()
    }
}

function remove(id) {
    let cart = getShoppingCart();

    if (cart[id]) {
        delete cart[id]
        saveCart(cart)
        location.reload()
    }
}

