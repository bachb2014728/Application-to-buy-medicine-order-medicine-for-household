export function formatPrice(number){
    const config = {
        style: 'currency',
        currency: 'VND',
        maximumFractionDigits: 9
    };
    return new Intl.NumberFormat('vi-VN', config).format(number);
}
export function formatPercentage(price,sale){
    if (sale ===0){
        return false;
    }else{
        const percent = 1-(sale / price);
        if (percent === 0){
            return false;
        }
        return Math.round(percent * 100) + "%";
    }
}