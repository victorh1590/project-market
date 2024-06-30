import { SigninFormData } from "../SigninForm/SigninFormTypes";

export type UserInfoData = {
    [K in keyof SigninFormData]: { label: string; value: string };
};