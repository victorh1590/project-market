import { useQuery } from "@tanstack/react-query"
import { UserInfoData } from "./UserInfoTypes";
import { Fragment } from "react"

export const UserInfo = () => {
    const getUser = (): Promise<UserInfoData> => {
        return new Promise((resolve) => {
            setTimeout(() => {
                resolve({
                    nome: { label: "Nome", value:"test" },
                    email: { label: "Email", value:"test@example.com" },
                    senha: { label: "Senha", value:"********" },
                    setor: { label: "Setor", value: "some" },
                });
            }, 10000);
        });
    };

    const { isPending, isError, data, error }  = useQuery({
        queryKey: ["user"],
        queryFn: getUser
    });

    if(isPending) {
        return (<span>Loading...</span>)
    } else if(isError) {
        return (
            <span>
                Error retrieving data.
                {error.message && <><br/>{error.message}</>}
            </span>
        )
    } else {
        const retrievedData = Object.entries(data).map(([key, value]) => (
            <Fragment key={`user-info-${key}`}>
                <span>
                    {`${value.label}: ${value.value}`}
                </span>
                <br/>
            </Fragment>
        ));
        
        return (
            <>
                {retrievedData}
            </>
        );
    }
}