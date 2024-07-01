import { ReactNode } from 'react';

interface GridProps {
    children: ReactNode;
}

export const Grid = ({ children }: GridProps) => {
    return (
        <div className="grid grid-cols-3 grid-rows-3 gap-0 min-h-screen min-w-screen overflow-hidden">
            {/* Cell 1 */}
            <div className="col-start-1 col-span-1 row-start-1 row-span-1 bg-gray-200 flex justify-center items-center h-full w-full">
                Cell 1
            </div>
            
            {/* Cell 2 */}
            <div className="col-start-2 col-span-1 row-start-1 row-span-1 bg-blue-200 flex justify-center items-center h-full w-full">
                Cell 2
            </div>
            
            {/* Cell 3 */}
            <div className="col-start-3 col-span-1 row-start-1 row-span-1 bg-green-200 flex justify-center items-center h-full w-full">
                Cell 3
            </div>
            
            {/* Cell 4 */}
            <div className="col-start-1 col-span-1 row-start-2 row-span-1 bg-yellow-200 flex justify-center items-center h-full w-full">
                Cell 4
            </div>
            
            {/* Cell 5 */}
            <div className="col-start-2 col-span-1 row-start-2 row-span-1 bg-red-200 flex justify-center items-center h-full w-full">
                Cell 5
            </div>
            
            {/* Cell 6 */}
            <div className="col-start-3 col-span-1 row-start-2 row-span-1 bg-purple-200 flex justify-center items-center h-full w-full">
                Cell 6
            </div>
            
            {/* Cell 7 */}
            <div className="col-start-1 col-span-1 row-start-3 row-span-1 bg-indigo-200 flex justify-center items-center h-full w-full">
                Cell 7
            </div>
            
            {/* Cell 8 */}
            <div className="col-start-2 col-span-1 row-start-3 row-span-1 bg-pink-200 flex justify-center items-center h-full w-full">
                Cell 8
            </div>
            
            {/* Cell 9 */}
            <div className="col-start-3 col-span-1 row-start-3 row-span-1 bg-orange-200 flex justify-center items-center h-full w-full">
                Cell 9
            </div>
            {children}

        </div>
    );
};

export default Grid;
