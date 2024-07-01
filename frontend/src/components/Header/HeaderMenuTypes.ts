// Define the structure of each menu entry
export type MenuEntry = {
    name: string;
    href: string;
};
  
export type HeaderMenuEntriesKvp = Record<string, MenuEntry>;

export type HeaderMenuEntries = {
    home: MenuEntry;
    tickets: MenuEntry;
    users: MenuEntry;
    about: MenuEntry;
};
