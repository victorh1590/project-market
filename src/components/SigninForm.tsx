import React from 'react';
import './SigninForm.css';
import { useForm } from 'react-hook-form';

function SigninForm() {
    const { register, handleSubmit, formState: { errors } } = useForm({
        mode: "onSubmit",
        reValidateMode: "onSubmit",
        defaultValues: {
            Nome: "",
            Login: "example@example.com",
            Senha: "",
            Setor: ""
        }
    });

    const onSubmit = (data : any) => {
        console.log(data);
    };

    return (
        <>
            <form onSubmit={handleSubmit(onSubmit)}>
                <label>Nome: </label>
                <input type="text" {...register("Nome", { required: `${"Nome"} é um campo requerido.` })} />
                <p>{errors.Nome?.message}</p>

                <label>Login: </label>
                <input type="email" {...register("Login", { required: `${"Login"} é um campo requerido.` })} />
                <p>{errors.Login?.message}</p>

                <label>Senha: </label>
                <input type="password" {...register("Senha", { required: `${"Senha"} é um campo requerido.` })} />
                <p>{errors.Senha?.message}</p>
                
                <label>Setor: </label>
                <select {...register("Setor", { required: `${"Setor"} é um campo requerido.` })}>
                    <option value="">Selecione uma opção.</option>
                    <option value="RH">RH</option>
                    <option value="TI">TI</option>
                    <option value="Contabilidade">Contabilidade</option>
                    <option value="Logística">Logística</option>
                    <option value="Compras">Compras</option>
                    <option value="Diretoria">Diretoria</option>
                    <option value="Vendas">Vendas</option>
                    <option value="Oficina">Oficina</option>
                </select>
                <p>{errors.Setor?.message}</p>
                <br/><br/>
                
                <button type="submit">Submit</button>
            </form>
        </>
    );
}

export default SigninForm;
