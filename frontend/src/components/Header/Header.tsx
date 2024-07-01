import { HeaderMenu } from "./HeaderMenu"


export const Header = () => {
    return <>
        <header 
            className="
            col-start-1 col-span-3
            row-start-1 row-span-1
            bg-red-200 flex flex-row
            justify-center items-center 
            h-full w-full bg-red-400"
        >
            <HeaderMenu></HeaderMenu>
        </header>
    </>
}