import { PROFILE } from '../auth/profile.enum';

export class User {
    private _id: number;
    private _login: string;
    private _lastname: string;
    private _firstname: string;
    private _email: string;
    private _profile: PROFILE;
    private _tenant: string;
    private _active: boolean;
    private _jwt: string|null;
    private _password: string|null = null;

    constructor(data: any) {
        this._id = data.id;
        this._login = data.login;
        this._lastname = data.lastname;
        this._firstname = data.firstname;
        this._email = data.email;
        this._profile = data.profile;
        this._tenant = data.tenant;
        this._active = data.active;
        this._jwt = data.jwt; // on login return jwt token
        // password is never read
    }

    public get id(): number {
        return this._id;
    }

    public set id(id: number) {
        this._id = id;
    }

    public get lastname(): string {
        return this._lastname;
    }

    public set lastname(lastname: string) {
        this._lastname = lastname;
    }

    public get login(): string {
        return this._login;
    }

    public set login(login: string) {
        this._login = login;
    }

    public get firstname(): string {
        return this._firstname;
    }

    public set firstname(n: string) {
        this._firstname = n;
    }

    public get email(): string {
        return this._email;
    }

    public set email(e: string) {
        this._email = e;
    }

    public get profile(): PROFILE {
        return this._profile;
    }

    public set profile(p: PROFILE) {
        this._profile = p;
    }

    public get tenant(): string {
        return this._tenant;
    }

    public set tenant(t: string) {
        this._tenant = t;
    }

    public get active(): boolean {
        return this._active;
    }

    public set active(b: boolean) {
        this._active = b;
    }

    public get jwtToken(): string|null {
        return this._jwt;
    }

    public set password(p: string|null) {
        this._password = p;
    }
    public get password(): string|null {
        return this._password;
    }

    public clearJwtToken(): void {
        this._jwt = null;
    }

    public allow(p: PROFILE): boolean {
        if (p == PROFILE.CUSTOMER) {
            return true;
        }
        if (p == PROFILE.OPERATOR) {
            return this._profile == PROFILE.ADMIN || this._profile == PROFILE.OPERATOR;
        }
        if (p == PROFILE.ADMIN) {
            return this._profile == PROFILE.ADMIN;
        }
        return false;
    }

    toJSON(): any {
       // console.log("this._password :"+this._password);
       if (this._password != null) {
            return { id: this.id, tenant: this.tenant, login: this.login, lastname: this.lastname,
                firstname: this.firstname, active: this.active, email: this.email, profile: this.profile, password: this.password};
        } else {
            return { id: this.id, tenant: this.tenant, login: this.login, lastname: this.lastname,
            firstname: this.firstname, active: this.active, email: this.email, profile: this.profile};
        }
    }
}
