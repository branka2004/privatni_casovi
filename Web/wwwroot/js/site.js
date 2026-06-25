const notifications = [];

function updateNotifications() {

    const badge =
        document.getElementById(
            "notificationBadge");

    const list =
        document.getElementById(
            "notificationList");

    if (!badge || !list)
        return;

    if (notifications.length === 0) {

        badge.style.display = "none";

        list.innerHTML =
            `
            <li>
                <span class="dropdown-item-text text-muted">
                    Nema notifikacija
                </span>
            </li>
            `;

        return;
    }

    badge.style.display = "inline";

    badge.innerText =
        notifications.length;

    list.innerHTML = "";

    notifications
        .slice()
        .reverse()
        .forEach(x => {

            list.innerHTML +=
                `
                <li>
                    <span class="dropdown-item-text">
                        ${x}
                    </span>
                </li>
                `;
        });
}

const connection =
    new signalR.HubConnectionBuilder()
        .withUrl(
            "https://localhost:7076/notificationHub")
        .build();

connection.on(
    "ReceiveNotification",
    function (message) {

        notifications.push(message);

        updateNotifications();
    });

connection.start()
    .then(() => {

        console.log("SignalR povezan.");

        const korisnikId =
            document.body.dataset.korisnikid;

        if (korisnikId) {

            connection.invoke(
                "RegisterUser",
                parseInt(korisnikId));
        }
    })
    .catch(err => {

        console.error(err);
    });

updateNotifications();