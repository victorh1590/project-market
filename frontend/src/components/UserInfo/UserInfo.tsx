import { useQuery, useQueryClient } from "@tanstack/react-query"
import { SigninFormData } from "../SigninForm/SigninFormTypes"
import { SigninForm } from "../SigninForm/SigninForm";

export const UserInfo = () => {
    const queryClient = useQueryClient()

    const getUser = (): Promise<SigninFormData> => {
        return new Promise((resolve) => {
            setTimeout(() => {
                resolve({
                    nome: "test",
                    email: "test@example.com",
                    senha: "********",
                    setor: "test"
                });
            }, 10);
        });
    };

    const { isPending, isError, data, error }  = useQuery({
        queryKey: ["user"],
        queryFn: getUser
    });

    return (
        <>
            <div>
                <span></span>
            </div>
        </>
    )
}