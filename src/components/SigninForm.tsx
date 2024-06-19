import './SigninForm.css'
import { useForm } from 'react-hook-form'

function SigninForm() {
    const nomeField : string = "Nome";
    const loginField : string = "Login";
    const senhaField : string = "Senha";
    const setorField : { fieldName : string, values : string[] } = { fieldName: "Setor", values: [] };

    const { register, handleSubmit, watch, formState: { errors } } = useForm({
        defaultValues: {
            Nome: "",
            Login: "example@example.com",
            Senha: "",
            Setor: ""
        }
    });


    return <>
            <form onSubmit={handleSubmit((data) => {
                console.log(data);
            })}>
                <label>Nome: </label>
                <input type="text" {...register("Nome", { required: `${"Nome"} é um campo requerido.` })}/>
                <p>{errors.Nome?.message?.toString()}</p>

                <label>Login: </label>
                <input type="email" {...register("Login", { required: `${"Login"} é um campo requerido.` })}/>
                <p>{errors.Login?.message?.toString()}</p>

                <label>Senha: </label>
                <input type="password" {...register("Senha", { required: `${"Senha"} é um campo requerido.` })}/>
                <p>{errors.Senha?.message?.toString()}</p>
                
                <label>Setor: </label>
                <select {...register("Setor", { required: `${"Setor"} é um campo requerido.` })}>
                    <option value="">Selecione uma opção.</option>
                    // TODO: Setores têm que ser alimentados via API.
                    <option value="RH">RH</option>
                    <option value="TI">TI</option>
                    <option value="Contabilidade">Contabilidade</option>
                    <option value="Logística">Logística</option>
                    <option value="Compras">Compras</option>
                    <option value="Diretoria">Diretoria</option>
                    <option value="Vendas">Vendas</option>
                    <option value="Oficina">Oficina</option>
                </select>
                <p>{errors.Setor?.message?.toString()}</p>
                <br/><br/>
                
                <button type="submit">Submit</button>
            </form>
        </>
}

export default SigninForm;