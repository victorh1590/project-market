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

const signinFormInputFields: Record<string, FormFieldProps> = {
    nome: {
        name: "nome",
        label: "Nome",
        type: "text",
        placeholder: "Digite seu nome",
    } as FormFieldProps,
    email: {
        name: "email",
        label: "Email",
        type: "email",
        placeholder: "Digite seu email",
    } as FormFieldProps,
    senha: {
        name: "senha",
        label: "Senha",
        type: "password",
        placeholder: "Digite sua senha",
    } as FormFieldProps
};

const signinFormSelectionFields: Record<string, FormSelectionProps> = {
    setor: {
        name: "setor",
        label: "Setor",
        multiple: false,
        options: setorOptions,
        defaultValue: setorOptions[0],
    } as FormSelectionProps
}

export const SigninForm = () => {
    const defaultValues : SigninFormData = {
        nome : "",
        email : "example@example.com",
        senha : "",
        setor : "Selecione uma opção."
    }


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
            <form onSubmit={handleSubmit(onSubmit)}>
                <FormField 
                    label={signinFormInputFields.nome.label}
                    type={signinFormInputFields.nome.type}
                    placeholder={defaultValues.nome}
                    name={signinFormInputFields.nome.name}
                    register={register}
                    error={errors.nome}
                />
                <br/>
                <FormField 
                    label={signinFormInputFields.email.label}
                    type={signinFormInputFields.email.type}
                    placeholder={defaultValues.email}
                    name={signinFormInputFields.email.name}
                    register={register}
                    error={errors.email}
                />
                <br/>
                <FormField 
                    label={signinFormInputFields.senha.label}
                    type={signinFormInputFields.senha.type}
                    placeholder={defaultValues.senha}
                    name={signinFormInputFields.senha.name}
                    register={register}
                    error={errors.senha}
                />
                <br/>
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
                <button type="submit">Submit</button>
            </form>
        </>
    );
}