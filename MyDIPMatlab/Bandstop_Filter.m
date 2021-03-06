function [G] = Bandstop_Filter(F,d,w)
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
H = zeros(P,Q);
H((D > d - w/2 & D < d + w/2)) = 1;
G = H .* F;
end