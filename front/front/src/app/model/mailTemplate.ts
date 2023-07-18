
export class MailTemplate {
    _id: number;
    _mailAcknowledge: string;
    _mailCompleted: string;
    
    constructor(data: any) {
        this._id = data.id;
        this._mailAcknowledge = data.mail_acknowledge;
        this._mailCompleted = data.mail_completed;
    }

    public get id(): number {
        return this._id;
    }

    public set id(id: number) {
        this._id = id;
    }
    
    public get mailAcknowledge(): string {
        return this._mailAcknowledge;
    }

    public set mailAcknowledge(s: string) {
        this._mailAcknowledge = s;
    }

    public get mailCompleted(): string {
        return this._mailCompleted;
    }

    public set mailCompleted(s: string) {
        this._mailCompleted = s;
    }


    toJSON(): any {
        return { id: this._id, mail_acknowledge: this._mailAcknowledge, mail_completed: this._mailCompleted};
    }
}