import {HeaderMenuEntries} from "./HeaderMenuTypes"

const headerMenuEntries: HeaderMenuEntries = {
    home: { name: "Home", href: "/" },
    tickets: { name: "Tickets", href: "/" },
    users: { name: "Usuários", href: "/" },
    about: { name: "About", href: "/about" }
};

export const HeaderMenu = () => {
    const links = Object.entries(headerMenuEntries).map(([key, value]) => (
        <a 
            className="
                flex
            "
            key={`${key}-menu-entry`} href={value.href}
        >{value.name}</a>
    ));

    return (
        <div 
            className="
            col-start-1 col-span-2
            row-start-1 row-span-1
            flex flex-row
            ml-10 items-center
            h-full w-full"
        >
            {links}
        </div>
    );
}