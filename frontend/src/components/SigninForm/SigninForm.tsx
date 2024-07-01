import './SigninForm.css';
import { useForm } from 'react-hook-form';
import { FormFieldProps, FormSelectionProps, SigninFormData, ValidFieldNames, ValidSelectionNames } from "./SigninFormTypes";
import { FormField } from "./FormField";
import { FormSelection } from './FormSelection';
import { zodResolver } from "@hookform/resolvers/zod";
import { createUserSchema } from './FormSchema';

const setorOptions = [
    "Selecione uma opção.",
    "RH",
    "TI",
    "Contabilidade",
    "Logística",
    "Compras",
    "Diretoria",
    "Vendas",
    "Oficina",
];

const defaultValues: SigninFormData = {
    nome: "",
    email: "example@example.com",
    senha: "",
    setor: setorOptions[0]
}

const signinFormInputFields: Record<string, FormFieldProps> = {
    nome: {
        name: "nome",
        label: "Nome",
        type: "text",
        defaultValue: defaultValues.nome,
    } as FormFieldProps,
    email: {
        name: "email",
        label: "Email",
        type: "email",
        defaultValue: defaultValues.email,
    } as FormFieldProps,
    senha: {
        name: "senha",
        label: "Senha",
        type: "password",
        defaultValue: defaultValues.senha,
    } as FormFieldProps
};

const signinFormSelectionFields: Record<string, FormSelectionProps> = {
    setor: {
        name: "setor",
        label: "Setor",
        multiple: false,
        options: setorOptions,
        defaultValue: defaultValues.setor,
    } as FormSelectionProps
}

export const SigninForm = () => {
    const { register, handleSubmit, formState: { errors } } = useForm<SigninFormData>({
        mode: "onSubmit",
        reValidateMode: "onSubmit",
        resolver: zodResolver(createUserSchema(defaultValues))
    });

    const onSubmit = (data : any) => {
        console.log("SUCCESS", data);
    };

    return (
        <>
            <form 
                className="col-start-2 col-span-1 row-start-2 row-span-1 bg-red-200 flex flex-col justify-center items-center h-full w-full bg-gray-400" 
                onSubmit={handleSubmit(onSubmit)}
            >
                <FormField
                    label={signinFormInputFields.nome.label}
                    type={signinFormInputFields.nome.type}
                    defaultValue={signinFormInputFields.nome.defaultValue}
                    name={signinFormInputFields.nome.name}
                    register={register}
                    error={errors.nome}
                />
                <FormField 
                    label={signinFormInputFields.email.label}
                    type={signinFormInputFields.email.type}
                    defaultValue={signinFormInputFields.email.defaultValue}
                    name={signinFormInputFields.email.name}
                    register={register}
                    error={errors.email}
                />
                <FormField 
                    label={signinFormInputFields.senha.label}
                    type={signinFormInputFields.senha.type}
                    defaultValue={signinFormInputFields.senha.defaultValue}
                    name={signinFormInputFields.senha.name}
                    register={register}
                    error={errors.senha}
                />
                <FormSelection
                    label={signinFormSelectionFields.setor.label}
                    multiple={signinFormSelectionFields.setor.multiple}
                    options={signinFormSelectionFields.setor.options}
                    defaultValue={signinFormSelectionFields.setor.defaultValue}
                    name={signinFormSelectionFields.setor.name}
                    register={register}
                    error={errors.setor}
                />
                <br/>
                <button type="submit" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                    Submit
                </button>
            </form>
        </>
    );
}