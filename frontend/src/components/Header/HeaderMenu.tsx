import {HeaderMenuEntries} from "./HeaderMenuTypes"

const headerMenuEntries: HeaderMenuEntries = {
    home: { name: "Home", href: "/" },
    tickets: { name: "Tickets", href: "/" },
    users: { name: "Usuários", href: "/" },
    about: { name: "About", href: "/about" }
};

export const HeaderMenu = () => {
    const links = Object.entries(headerMenuEntries).map(([key, value]) => (
        <a key={`${key}-menu-entry`} href={value.href}>{value.name}</a>
    ));

    return (
        <div>
            {links}
        </div>
    );
}