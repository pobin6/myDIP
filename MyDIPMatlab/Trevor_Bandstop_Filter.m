function [G] = Trevor_Bandstop_Filter(F,d,w,n)
%Low_Pass_Filter ��ͨ�˲���
%���� F ����Ҷ�任��ľ���
%���� d ��ֹƵ��
% ����
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
% ����
D = ((u-P/2).^2 + (v-Q/2).^2).^0.5;
% ��ͨ�˲�����ֵ
H = 1./(1 + (D*w./(D.^2 - d^2)).^(2*n));
G = H .* F;
figure;
imshow(real(fftshift(G)),[]);
end
 