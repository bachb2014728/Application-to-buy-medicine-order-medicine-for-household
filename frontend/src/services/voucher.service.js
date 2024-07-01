import createApiClient from "./api.service.js";
class VoucherClientService {
    constructor(baseUrl = "http://localhost:5086/api/v1/vouchers") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    async getAll() {
        return (await this.api.get("/", {}));
    }
    async create(data){
        return (await this.api.post("/",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async getOne(id){
        return (await this.api.get(`/${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async update(id,data){
        return (await this.api.put(`/${id}`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async deleteOne(data){
        return (await this.api.delete(`/${data}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async addToVoucher(data){
        return (await this.api.put("/add-to-voucher",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async myVoucher(){
        return (await this.api.get("/my-list-voucher",{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async getAllVoucherOfStore(id){
        return (await this.api.get(`/get-all-voucher-of-store/${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
}
export default new VoucherClientService();