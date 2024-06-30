import { z, ZodType } from "zod";
import { SigninFormData } from "./SigninFormTypes"

export const createUserSchema = (defaultValue: SigninFormData): ZodType<SigninFormData> => {
    return z.object({
        nome: z
            .string({ required_error: "\"Nome\" é um campo obrigatório." })
            .min(3, { message: "\"Nome\" deve ter mais de 3 caracteres." })
            .max(32, { message: "\"Nome\" ultrapassou o limite de caracteres." }),
        email: z
            .string({required_error: "\"Email\" é um campo obrigatório."})
            .email({ message: "\"Email\" inválido." })
            .refine(
                (data) => data !== defaultValue.setor,
                { message: "O campo \"Email\" deve ser diferente do valor padrão." }
            ),
        senha: z
            .string({ required_error: "\"Senha\" é um campo obrigatório." })
            .min(8, { message: "\"Senha\" deve ter pelo menos 8 caracteres." })
            .max(32, { message: "\"Senha\" ultrapassou o limite de caracteres." }),
        setor: z
        .string()
        .refine(
            (data) => data !== defaultValue.setor,
            { message: 'O campo "setor" deve ser diferente do valor padrão.' }
        )
    });
};