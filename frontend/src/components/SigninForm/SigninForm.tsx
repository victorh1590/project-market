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
            <form onSubmit={handleSubmit(onSubmit)}>
                <FormField 
                    label={signinFormInputFields.nome.label}
                    type={signinFormInputFields.nome.type}
                    defaultValue={signinFormInputFields.nome.defaultValue}
                    name={signinFormInputFields.nome.name}
                    register={register}
                    error={errors.nome}
                />
                <br/>
                <FormField 
                    label={signinFormInputFields.email.label}
                    type={signinFormInputFields.email.type}
                    defaultValue={signinFormInputFields.email.defaultValue}
                    name={signinFormInputFields.email.name}
                    register={register}
                    error={errors.email}
                />
                <br/>
                <FormField 
                    label={signinFormInputFields.senha.label}
                    type={signinFormInputFields.senha.type}
                    defaultValue={signinFormInputFields.senha.defaultValue}
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