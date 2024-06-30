import './SigninForm.css';
import { useForm } from 'react-hook-form';
import { FormData } from "./SigninFormTypes";
import FormField from "./FormField";
import FormSelection from './FormSelection';
import { zodResolver } from "@hookform/resolvers/zod";
import { createUserSchema } from './FormSchema';

function SigninForm() {
    const defaultValues : FormData = {
        nome : "",
        email : "example@example.com",
        senha : "",
        setor : "Selecione uma opção."
    }

    const setorOptions = [
        defaultValues.setor, 
        "RH",
        "TI", 
        "Contabilidade", 
        "Logística",
        "Compras",
        "Diretoria",
        "Vendas",
        "Oficina"
    ]

    const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
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
                    label="Nome"
                    type="text" 
                    placeholder={defaultValues.nome}
                    name="nome"
                    register={register}
                    error={errors.nome}
                />
                <br/>
                <FormField 
                    label="Email"
                    type="email" 
                    placeholder={defaultValues.email}
                    name="email"
                    register={register}
                    error={errors.email}
                />
                <br/>
                <FormField 
                    label="Senha"
                    type="text" 
                    placeholder={defaultValues.senha}
                    name="senha"
                    register={register}
                    error={errors.senha}
                />
                <br/>
                <FormSelection
                    label="Setor"
                    multiple={false}
                    options={setorOptions}
                    defaultValue={defaultValues.setor}
                    name="setor"
                    register={register}
                    error={errors.setor}
                />
                <br/>
                <button type="submit">Submit</button>
            </form>
        </>
    );
}

export default SigninForm;
