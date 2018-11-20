function [G] = Notch_Filter(F,D0,n)
%Low_Pass_Filter 低通滤波器
%参数 F 傅里叶变换后的矩阵
%参数 d 截止频率
% 坐标
[P,Q] = size(F);
if P>Q
    Max = P;
else
    Max = Q;
end
u = ones(Max,1) * (1:Max);
u = u(1:P,1:Q);
v = (1:Max)' * ones(1,Max);
v = v(1:P,1:Q);
% 计算
uk = P/2;
vk = Q/2;
Dk = ((u-P/2-uk)^2 + (v-Q/2-vk)^2)^0.5;
D_k = ((u-P/2+uk)^2 + (v-Q/2+vk)^2)^0.5;
% 低通滤波器阈值
H = zeros(P,Q);
H(D < D0) = 1;
G = H .* F;
end